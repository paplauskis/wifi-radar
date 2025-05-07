<template>
    <v-container class="py-12">
        <v-row justify="center">
            <v-col cols="12" md="6">
                <v-card class="pa-6" elevation="4">
                    <v-card-title class="font-weight-bold text-h5 mb-4">
                        My Profile
                    </v-card-title>

                    <v-card-text>
                        <v-list>
                            <v-list-item>
                                <v-list-item-title>Name</v-list-item-title>
                                <v-list-item-subtitle>{{ user.name }}</v-list-item-subtitle>
                            </v-list-item>

                            <v-list-item>
                                <v-list-item-title>Email</v-list-item-title>
                                <v-list-item-subtitle>{{ user.email }}</v-list-item-subtitle>
                            </v-list-item>

                            <v-divider class="my-4"></v-divider>

                            <v-list-item>
                                <v-list-item-title>Favorites</v-list-item-title>
                                <v-list-item-subtitle>
                                    <v-chip v-for="wifi in user.favorites" :key="wifi" class="ma-1" color="primary"
                                        label outlined>
                                        <span class="font-weight-bold">{{ wifi }}</span>
                                    </v-chip>
                                </v-list-item-subtitle>
                            </v-list-item>

                            <v-divider class="my-4"></v-divider>

                            <v-list-item>
                                <v-list-item-title>Shared Passwords</v-list-item-title>
                                <v-list-item-subtitle>
                                    <v-chip v-for="entry in user.sharedPasswords" :key="entry.wifiId" class="ma-1"
                                        color="green" label outlined>
                                        <span class="font-weight-bold">{{ entry.wifiId }}: {{ entry.password }}</span>
                                    </v-chip>
                                </v-list-item-subtitle>
                            </v-list-item>
                        </v-list>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
const user = {
    name: 'Jonas Varnas',
    email: 'jonas@gmail.com',
    favorites: ['wifi001', 'wifi789', 'wifi456'],
    sharedPasswords: [
        { wifiId: 'wifi123', password: 'cafe123' },
        { wifiId: 'wifi456', password: 'public456' }
    ]
}
</script>