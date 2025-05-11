<template>
  <div>
    <v-list>
      <v-list-item v-for="(entry, index) in localPasswords" :key="index">
        <v-row class="w-100" no-gutters align="center">
          <v-col>
            <p class="text-subtitle-1 font-weight-medium mb-0">🔒 {{ entry.password }}</p>
            <p class="text-caption text-grey mb-0">
              Added {{ formatDate(entry.dateAdded) }} · Confirmed by
              {{ entry.confirmedCount }} users
            </p>
          </v-col>

          <v-col cols="auto" class="mr-4">
            <v-btn icon size="small" @click="confirm(entry)">
              <v-icon color="primary">mdi-thumb-up</v-icon>
            </v-btn>
          </v-col>
        </v-row>
      </v-list-item>

      <v-list-item v-if="!localPasswords.length" class="text-center">
        <v-list-item-title class="text-grey"> No passwords shared yet </v-list-item-title>
      </v-list-item>
    </v-list>

    <v-divider class="my-4" />

    <v-form @submit.prevent="submit">
      <v-text-field
        v-model="newPassword"
        label="Share a new password"
        prepend-inner-icon="mdi-lock"
        variant="outlined"
        dense
        required
      />
      <v-btn type="submit" color="primary" block>Submit Password</v-btn>
    </v-form>
  </div>
</template>

<script setup>
import { defineEmits, ref, onMounted } from 'vue'

const emit = defineEmits(['confirm', 'submit'])

const localPasswords = ref([])
const newPassword = ref('')

const formatDate = (iso) =>
  new Date(iso).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })

// Inject dummy passwords (frontend-only)
onMounted(() => {
  localPasswords.value = [
    {
      password: 'cafe-wifi123',
      confirmedCount: 4,
      dateAdded: new Date(Date.now() - 86400000 * 3).toISOString(),
    },
    {
      password: 'freeaccess2024',
      confirmedCount: 0,
      dateAdded: new Date(Date.now() - 86400000 * 1).toISOString(),
    },
  ]
})

const confirm = (entry) => {
  entry.confirmedCount + emit('confirm', entry)
}

const submit = () => {
  const entry = {
    password: newPassword.value,
    confirmedCount: 0,
    dateAdded: new Date().toISOString(),
  }
  localPasswords.value.push(entry)
  emit('submit', entry)
  newPassword.value = ''
}
</script>
