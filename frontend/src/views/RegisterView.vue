<template>
    <v-container class="fill-height d-flex justify-center align-center">
        <v-card class="pa-6" width="420" elevation="8">
            <v-card-title class="justify-center">
                <h2 class="text-h5 font-weight-bold">Create an Account</h2>
            </v-card-title>

            <v-card-text>
                <v-form @submit.prevent="handleRegister" ref="form">
                    <v-text-field v-model="username" label="Username" prepend-inner-icon="mdi-account" required />

                    <v-text-field v-model="password" label="Password" type="password" prepend-inner-icon="mdi-lock"
                        required />

                    <v-text-field v-model="confirmPassword" label="Confirm Password" type="password"
                        prepend-inner-icon="mdi-lock-check" required />

                    <v-alert v-if="registerError" type="error" class="mb-2">{{ registerError }}</v-alert>

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

const username = ref('')
const password = ref('')
const confirmPassword = ref('')
const registerError = ref('')

const router = useRouter()

const canSubmit = computed(() => {
    return (
        username.value.trim() !== '' &&
        password.value !== '' &&
        confirmPassword.value !== '' &&
        password.value === confirmPassword.value
    )
})

const handleRegister = async () => {
    registerError.value = ''
    try {
        const res = await fetch('http://localhost:5274/api/user/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username: username.value, password: password.value }),
        })
        const data = await res.json()
        if (res.ok && data.success) {
            router.push('/login')
        } else {
            registerError.value = data.message || 'Registration failed'
        }
    } catch {
        registerError.value = 'Network error'
    }
}
</script>

<style scoped>
.v-card {
    max-width: 100%;
    border-radius: 16px;
}
</style>