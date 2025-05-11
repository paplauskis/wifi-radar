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
import { ref } from 'vue'
import WifiPasswordList from './WifiPasswordList.vue'
import WifiReviewList from './WifiReviewList.vue'

const props = defineProps({
  wifi: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['back'])

const isFavorite = ref(false)

const confirmPassword = (entry) => {
  entry.confirmedCount++
}

const sharePassword = (entry) => {
  console.log('Submitted password:', entry)
}

// Dummy
const reviews = ref([
  {
    user: 'Jonas P.',
    rating: 4,
    comment: 'Pretty fast internet and stable connection.',
    date: new Date().toISOString(),
  },
  {
    user: 'Agnė K.',
    rating: 2,
    comment: 'Had some connection drops.',
    date: new Date(Date.now() - 86400000).toISOString(),
  },
])

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
