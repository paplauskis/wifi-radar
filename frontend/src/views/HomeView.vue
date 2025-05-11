<template>
  <v-container
    class="fill-height d-flex flex-column justify-center align-center text-center py-16 px-4"
  >
    <v-row justify="center">
      <v-col cols="12">
        <h1 class="text-h2 font-weight-bold mb-2">WiFi Radar</h1>
        <p class="text-subtitle-1 mb-6">
          Explore and discover free public Wi-Fi hotspots near you.
        </p>
        <v-form>
          <v-btn color="primary" size="large" block @click="goToMap"> Start Exploring </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const searchCity = ref('')
const router = useRouter()

const goToMap = () => {
  router.push('/map')
}
</script>

<style scoped>
h1 {
  color: #2c3e50;
}
p {
  color: #666;
}
</style>
