<template>
  <div>
    <h4 class="text-subtitle-1 mb-2">User Reviews</h4>
    <v-list density="comfortable">
      <v-list-item v-for="(review, index) in reviews" :key="index" class="pb-2">
        <v-list-item-content>
          <div class="d-flex align-center justify-space-between mb-1">
            <div class="d-flex align-center">
              <span class="font-weight-medium me-3">{{ review.user }}</span>
              <v-rating v-model="review.rating" readonly color="amber" size="18" />
            </div>
            <span class="text-caption text-grey">{{ formatDate(review.date) }}</span>
          </div>
          <p class="text-body-2">{{ review.comment }}</p>
        </v-list-item-content>
      </v-list-item>

      <v-list-item v-if="!reviews.length">
        <v-list-item-title class="text-grey">No reviews yet</v-list-item-title>
      </v-list-item>
    </v-list>
  </div>
</template>

<script setup>
const props = defineProps({
  reviews: {
    type: Array,
    default: () => [],
  },
})

const formatDate = (iso) =>
  new Date(iso).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
</script>
