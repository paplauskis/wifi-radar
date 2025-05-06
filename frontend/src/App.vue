<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-toolbar-title>WiFi Radar</v-toolbar-title>
      <v-spacer />
      <v-btn to="/" text>Home</v-btn>
      <v-btn to="/map" text>Map</v-btn>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>
