<template>
  <v-list height="calc(100vh - 200px)" class="overflow-y-auto">
    <v-list-item
      v-for="(wifi, index) in wifiPoints"
      :key="index"
      :title="wifi.name"
      :subtitle="wifi.provider"
      @click="emit('select', wifi)"
    >
      <template #prepend>
        <v-icon color="primary">mdi-wifi</v-icon>
      </template>
    </v-list-item>
  </v-list>
</template>

<script setup>
defineProps({ wifiPoints: Array })
const emit = defineEmits(['select'])
</script>
