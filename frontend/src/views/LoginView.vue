<template>
    <v-container class="fill-height d-flex justify-center align-center">
        <v-card class="pa-6" width="400" elevation="8">
            <v-card-title class="justify-center">
                <h2 class="text-h5 font-weight-bold">Login to WiFi Radar</h2>
            </v-card-title>

            <v-card-text>
                <v-form @submit.prevent="handleLogin" ref="form">
                    <v-text-field v-model="username" label="Username" required prepend-inner-icon="mdi-account" />

                    <v-text-field v-model="password" label="Password" type="password" required
                        prepend-inner-icon="mdi-lock" />

                    <v-checkbox v-model="remember" label="Remember me" color="primary" />

                    <v-alert v-if="loginError" type="error" class="mb-2">{{ loginError }}</v-alert>

                    <v-btn color="primary" class="mt-4" type="submit" block>
                        Login
                    </v-btn>
                </v-form>
            </v-card-text>

            <v-card-actions class="justify-center">
                <span class="text-caption">Don't have an account?</span>
                <v-btn variant="text" color="primary" to="/register">Register</v-btn>
            </v-card-actions>
        </v-card>
    </v-container>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const username = ref('')
const password = ref('')
const remember = ref(false)
const loginError = ref('')

const router = useRouter()

const handleLogin = async () => {
  loginError.value = ''
  try {
    const res = await fetch('http://localhost:5274/api/user/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: username.value, password: password.value }),
    })
    const data = await res.json()
    if (res.ok && data.success && data.data?.accessToken) {
      localStorage.setItem('accessToken', data.data.accessToken)
      router.push('/map')
    } else {
      loginError.value = data.message || 'Login failed'
    }
  } catch {
    loginError.value = 'Network error'
  }
}
</script>

<style scoped>
.v-card {
    max-width: 100%;
    border-radius: 16px;
}
</style>