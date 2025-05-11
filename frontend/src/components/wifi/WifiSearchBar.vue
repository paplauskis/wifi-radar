<template>
  <v-text-field
    v-model="query"
    placeholder="Search by city..."
    prepend-inner-icon="mdi-magnify"
    append-inner-icon="mdi-close-circle"
    @click:append-inner="emitClear"
    dense
    variant="outlined"
  />
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue: String,
})

const emit = defineEmits(['update:modelValue', 'clear'])

const query = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})

const emitClear = () => emit('clear')
</script>
