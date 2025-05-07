<template>
  <v-container fluid class="fill-height pa-0">
    <v-row no-gutters class="fill-height">
      <v-col cols="12" md="4" class="pa-4 elevation-2" style="z-index: 2; background-color: white">
        <h2 class="text-h6 font-weight-bold mb-2">Nearby Wi-Fi Hotspots</h2>
        <v-text-field
          v-model="searchQuery"
          placeholder="Search by city..."
          prepend-inner-icon="mdi-magnify"
          dense
          variant="outlined"
        />

        <v-list height="calc(100vh - 200px)" class="overflow-y-auto">
          <v-list-item
            v-for="(wifi, index) in wifiPoints"
            :key="index"
            :title="wifi.name"
            :subtitle="wifi.provider"
            @click="focusHotspot(wifi)"
          >
            <template #prepend>
              <v-icon color="primary">mdi-wifi</v-icon>
            </template>
          </v-list-item>
        </v-list>
      </v-col>

      <v-col cols="12" md="8" class="map-container">
        <div id="map" class="fill-height"></div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'

const searchQuery = ref('')

const wifiPoints = ref([
  { id: 1, name: 'Cafe WiFi', lat: 54.6872, lng: 25.2797, password: 'coffeetime' },
  { id: 2, name: 'Library WiFi', lat: 54.6890, lng: 25.2765, password: null }
])

let map = null

onMounted(() => {
  map = L.map('map').setView([54.689, 25.2799], 14)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors'
  }).addTo(map)

  wifiPoints.value.forEach((wifi) => {
    L.marker([wifi.lat, wifi.lng])
      .addTo(map)
      .bindPopup(`<b>${wifi.name}</b><br>${wifi.provider}`)
  })
})

const focusHotspot = (wifi) => {
  map.setView([wifi.lat, wifi.lng], 16)
}
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
