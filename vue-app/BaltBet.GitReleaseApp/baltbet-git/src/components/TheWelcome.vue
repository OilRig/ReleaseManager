<script setup lang="ts">
import { useBitbucketDataStore } from "@/stores/bitbucket";

const store = useBitbucketDataStore();
</script>

<template>
  <VCard v-for="project in store.projects">
    <VCardTitle>{{ project.name }}</VCardTitle>
    <VCardText> </VCardText>
  </VCard>
</template>
