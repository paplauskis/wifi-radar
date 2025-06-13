<template>
  <v-container fluid class="fill-height pa-0">
    <v-row no-gutters class="fill-height">
      <v-col cols="12" md="4" class="pa-4 elevation-2" style="z-index: 2; background-color: white">
        <h2 class="text-h6 font-weight-bold mb-2">Nearby Wi-Fi Hotspots</h2>

        <v-row class="mt-2" dense>
          <WifiSearchBar v-model="searchQuery" @clear="clearSearch" />
        </v-row>

        <div v-if="!selectedWifi">
          <WifiList :wifiPoints="wifiPoints" @select="selectWifi" />
        </div>

        <div v-else>
          <WifiDetails :wifi="selectedWifi" @back="selectedWifi = null" />
        </div>
      </v-col>

      <v-col cols="12" md="8" class="map-container">
        <div id="map" class="fill-height"></div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import { geocodeCity } from '@/services/geocoding'

import WifiSearchBar from '@/components/wifi/WifiSearchBar.vue'
import WifiList from '@/components/wifi/WifiList.vue'
import WifiDetails from '@/components/wifi/WifiDetails.vue'

const searchQuery = ref('')
const wifiPoints = ref([])
const selectedWifi = ref(null)
const markers = []

let map = null

onMounted(() => {
  map = L.map('map').setView([54.689, 25.2799], 14)
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
  }).addTo(map)
})

const focusHotspot = (wifi) => {
  map.setView([wifi.lat, wifi.lng], 17)
}

const selectWifi = (wifi) => {
  selectedWifi.value = wifi
  focusHotspot(wifi)

  const popupHtml = `
    <div style="max-width: 250px;">
      <h3 style="margin-bottom: 4px;">${wifi.name}</h3>
      <p style="margin: 0;"><strong>Provider:</strong> ${wifi.provider}</p>
      <p style="margin: 0;"><strong>Address:</strong> ${wifi.address}</p>
      ${wifi.password ? `<p style="margin: 0;"><strong>Password:</strong> ${wifi.password}</p>` : ''}
    </div>
  `

  L.popup({
    offset: L.point(0, -20),
  })
    .setLatLng([wifi.lat, wifi.lng])
    .setContent(popupHtml)
    .openOn(map)
}

const clearSearch = () => {
  searchQuery.value = ''
  wifiPoints.value = []
  selectedWifi.value = null
  markers.forEach((m) => map.removeLayer(m))
  markers.length = 0
}

let debounceTimeout = null

watch(searchQuery, (newQuery) => {
  clearTimeout(debounceTimeout)
  if (newQuery.length < 3) return

  debounceTimeout = setTimeout(async () => {
    const result = await geocodeCity(newQuery)
    if (result) {
      map.setView([result.lat, result.lon], 14)

      // Get city name from geocode result
      const city = result.address?.city || result.display_name.split(',')[0]
      
      // Call our backend endpoint
      const response = await fetch(`http://localhost:5274/api/map/search?city=${encodeURIComponent(city)}&radius=2000`, {
        headers: {
          'Accept': 'application/json',
        },
      })
      
      if (!response.ok) {
        console.error('Failed to fetch WiFi data:', response.status, response.statusText)
        wifiPoints.value = []
        return
      }

      const data = await response.json()
      
      if (!data.success) {
        wifiPoints.value = []
        return
      }
          wifiPoints.value = data.data.map((wifi) => {
      const tags = wifi.tags || {};
      return {
        id: wifi.id,
        name: tags.name || 'Unnamed hotspot',
        provider: tags.operator || 'Unknown provider',
        password: tags['internet_access:ssid'] || null,
        address: [tags['addr:street'], tags['addr:housenumber'], tags['addr:city']].filter(Boolean).join(', ') || 'No address info',
        lat: wifi.lat,
        lng: wifi.lon,
        city: tags['addr:city'],
        street: tags['addr:street'],
        buildingNumber: tags['addr:housenumber'],
      }
    })

      window.addEventListener('select-wifi', (e) => {
        const id = e.detail
        const match = wifiPoints.value.find((w) => w.id === id)
        if (match) selectWifi(match)
      })

      wifiPoints.value.forEach((wifi) => {
        const popupHtml = `
          <div style="max-width: 250px;">
            <h3 style="margin-bottom: 4px;">${wifi.name}</h3>
            <p style="margin: 0;"><strong>Provider:</strong> ${wifi.provider}</p>
            <p style="margin: 0;"><strong>Address:</strong> ${wifi.address}</p>
            ${wifi.password ? `<p style="margin: 0;"><strong>Password:</strong> ${wifi.password}</p>` : ''}
            <p style="margin-top: 6px;">
              <a href="#" onclick="window.dispatchEvent(new CustomEvent('select-wifi', { detail: ${wifi.id} }))">
                View more details →
              </a>
            </p>
          </div>
        `
        const marker = L.marker([wifi.lat, wifi.lng]).addTo(map).bindPopup(popupHtml)
        markers.push(marker)
      })
    }
  }, 500)
})
</script>

<style scoped>
.map-container {
  position: relative;
  z-index: 1;
}
#map {
  height: 100%;
  width: 100%;
  z-index: 1;
}
</style>
