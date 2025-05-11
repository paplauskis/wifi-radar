<template>
  <v-container class="fill-height d-flex justify-center align-center text-center">
    <div>
      <h1 class="text-h5 font-weight-bold">Backend Test</h1>
      <v-btn color="primary" @click="pingBackend" class="mt-4">Ping Backend</v-btn>

      <div v-if="result" class="mt-4">
        <v-alert
          :type="result.status === 'backend-ok' ? 'success' : 'error'"
          border="start"
          prominent
        >
          {{ result.status }} – {{ result.time }}
        </v-alert>
      </div>
    </div>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'

const result = ref(null)

const pingBackend = async () => {
  try {
    const res = await fetch('http://localhost:5274/api/ping')
    result.value = await res.json()
  } catch (err) {
    result.value = { status: 'error', time: new Date().toISOString() }
  }
}
</script>
