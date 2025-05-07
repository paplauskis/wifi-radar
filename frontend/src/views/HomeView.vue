<template>
  <v-container class="fill-height d-flex flex-column justify-center align-center text-center py-16 px-4">
    <v-row justify="center">
      <v-col cols="12">
        <h1 class="text-h2 font-weight-bold mb-2">WiFi Radar</h1>
        <p class="text-subtitle-1 mb-6">
          Explore and discover free public Wi-Fi hotspots near you.
        </p>
        <v-form>
          <v-text-field
            v-model="searchCity"
            placeholder="Enter a city to find WiFi hotspots"
            prepend-inner-icon="mdi-magnify"
            variant="outlined"
            density="comfortable"
            color="primary"
            rounded
            hide-details
            class="mb-4"
            aria-label="City Search Input"
          />
          <v-btn color="primary" size="large" block @click="goToMap">
            Start Exploring
          </v-btn>
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
