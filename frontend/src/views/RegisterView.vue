<template>
    <v-container class="fill-height d-flex justify-center align-center">
        <v-card class="pa-6" width="420" elevation="8">
            <v-card-title class="justify-center">
                <h2 class="text-h5 font-weight-bold">Create an Account</h2>
            </v-card-title>

            <v-card-text>
                <v-form @submit.prevent="handleRegister" ref="form">
                    <v-text-field v-model="name" label="Full Name" prepend-inner-icon="mdi-account" required />

                    <v-text-field v-model="email" label="Email" type="email" prepend-inner-icon="mdi-email" required />

                    <v-text-field v-model="password" label="Password" type="password" prepend-inner-icon="mdi-lock"
                        required />

                    <v-text-field v-model="confirmPassword" label="Confirm Password" type="password"
                        prepend-inner-icon="mdi-lock-check" required />

                    <v-btn color="primary" type="submit" block class="mt-4" :disabled="!canSubmit">
                        Register
                    </v-btn>
                </v-form>
            </v-card-text>

            <v-card-actions class="justify-center">
                <span class="text-caption">Already have an account?</span>
                <v-btn variant="text" color="primary" to="/login">Login</v-btn>
            </v-card-actions>
        </v-card>
    </v-container>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')

const router = useRouter()

const canSubmit = computed(() => {
    return (
        name.value.trim() !== '' &&
        email.value.trim() !== '' &&
        password.value !== '' &&
        confirmPassword.value !== '' &&
        password.value === confirmPassword.value
    )
})

const handleRegister = () => {
    console.log('Registering:', name.value, email.value)
    router.push('/login')
}
</script>

<style scoped>
.v-card {
    max-width: 100%;
    border-radius: 16px;
}
</style>