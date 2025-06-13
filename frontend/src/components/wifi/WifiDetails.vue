<template>
  <div>
    <v-btn icon @click="$emit('back')">
      <v-icon>mdi-arrow-left</v-icon>
    </v-btn>

    <div class="d-flex align-center mt-2 mb-1">
      <v-icon color="primary" class="me-2">mdi-wifi</v-icon>
      <h3 class="text-h6 font-weight-bold">{{ wifi.name }}</h3>
    </div>

    <p class="mb-1"><strong>Provider:</strong> {{ wifi.provider }}</p>
    <p class="mb-4"><strong>Address:</strong> {{ wifi.address }}</p>

    <v-switch v-model="isFavorite" color="pink" label="Favorite" class="my-6" inset />

    <v-divider class="my-4" />

    <h4 class="text-subtitle-1 mb-2">Shared Passwords</h4>
    <WifiPasswordList @confirm="confirmPassword" @submit="sharePassword" />

    <v-divider class="my-4" />

    <WifiReviewList :reviews="reviews" />

    <v-form @submit.prevent="submitReview" class="mt-4">
      <v-rating v-model="newReview.rating" color="amber" dense background-color="grey lighten-2" />
      <v-textarea
        v-model="newReview.comment"
        label="Write your review"
        auto-grow
        variant="outlined"
        class="my-2"
        rows="2"
        dense
        required
      />
      <v-btn type="submit" color="primary" block>Submit Review</v-btn>
    </v-form>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import WifiPasswordList from './WifiPasswordList.vue'
import WifiReviewList from './WifiReviewList.vue'

const props = defineProps({
  wifi: {
    type: Object,
    required: true,
  },
})

const isFavorite = ref(false)

const confirmPassword = (entry) => {
  entry.confirmedCount++
}

const sharePassword = (entry) => {
  console.log('Submitted password:', entry)
}

const reviews = ref([])

async function fetchReviews() {
  if (!props.wifi) return
  // Extract city, street, buildingNumber from wifi.address or wifi object
  const { city, street, buildingNumber } = props.wifi
  if (!city || !street || !buildingNumber) return
  const url = `http://localhost:5274/api/wifi/reviews?city=${encodeURIComponent(city)}&street=${encodeURIComponent(street)}&buildingNumber=${encodeURIComponent(buildingNumber)}`
  try {
    const res = await fetch(url)
    const data = await res.json()
    if (res.ok && data.success && Array.isArray(data.data)) {
      reviews.value = data.data.map(r => ({
        user: r.user || 'Anonymous',
        rating: r.rating,
        comment: r.comment,
        date: r.date || new Date().toISOString(),
      }))
    } else {
      reviews.value = []
    }
  } catch {
    reviews.value = []
  }
}

onMounted(fetchReviews)
watch(() => props.wifi, fetchReviews, { immediate: true })

const newReview = ref({
  rating: 0,
  comment: '',
})

const submitReview = () => {
  if (newReview.value.rating && newReview.value.comment.trim()) {
    reviews.value.push({
      user: 'Anonymous',
      rating: newReview.value.rating,
      comment: newReview.value.comment,
      date: new Date().toISOString(),
    })
    newReview.value.rating = 0
    newReview.value.comment = ''
  }
}
</script>
