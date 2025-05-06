<template>
  <v-container class="fill-height d-flex flex-column justify-center align-center text-center">
    <v-row>
      <v-col>
        <h1 class="text-h2 font-weight-bold mb-4">WiFi Radar</h1>
        <p class="text-subtitle-1 mb-6">
          Explore and discover free public Wi-Fi hotspots near you.
        </p>
        <v-btn color="primary" large @click="goToMap">Start Exploring</v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { useRouter } from 'vue-router'
const router = useRouter()
const goToMap = () => router.push('/map')
</script>
