<script setup lang="ts">
import router from "@/router";
import { useBitbucketDataStore } from "@/stores/bitbucket";

const store = useBitbucketDataStore();

async function onProjectClick(projKey: string): Promise<void> {
  router.push({ name: "project", params: { key: projKey } });
}
</script>

<template>
  <VRow>
    <VCol>
      <VCard
        v-for="project in store.projects"
        :key="project.name"
        @click="onProjectClick(project.projectKey)"
      >
        <VCardTitle>{{ project.name }}</VCardTitle>
        <VCardText>
          <strong>Число репозиторий:</strong
          ><span>{{ project.repositories.length }}</span>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
</template>
