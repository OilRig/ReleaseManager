<script setup lang="ts">
import { useBitbucketDataStore } from "@/stores/bitbucket";
import router from "@/router";

const store = useBitbucketDataStore();

async function onRepositoryClick(projKey: string, slug: string): Promise<void> {
  router.push({ name: "repository", params: { key: projKey, slug: slug } });
}
</script>

<template>
  <VRow v-for="repo in store.project.repositories" :key="repo.name">
    <VCol md="12" cols="12">
      <VCard @click="onRepositoryClick(store.project.projectKey, repo.slug)">
        <VCardTitle
          ><strong>{{ repo.name }}</strong></VCardTitle
        >
        <VCardText>
          <strong>URL:</strong><span>{{ repo.link }}</span>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
</template>
