<script setup lang="ts">
import { useBitbucketDataStore } from "@/stores/bitbucket";
import router from "@/router";
import { reactive } from "vue";
import { storeToRefs } from "pinia";

async function createPull(
  key: string,
  slug: string,
  branchName: string
): Promise<void> {
  await store.createPullRequest(key, slug, branchName);

  await store.fetchRepository(key, slug);
}

async function mergePull(
  key: string,
  slug: string,
  id: number,
  branchName: string
): Promise<void> {
  await store.mergePullRequest(key, slug, id, branchName);

  await store.fetchRepository(key, slug);
}
const store = useBitbucketDataStore();

const { projectRepository } = storeToRefs(useBitbucketDataStore());
let resReact = reactive({ projectRepository });
</script>

<template>
  <VContainer>
    <VRow
      v-for="branch in resReact.projectRepository.releaseBranches"
      :key="branch.name"
    >
      <VCol md="12" cols="12">
        <VCard>
          <VCardTitle
            ><strong>{{ branch.name }}</strong></VCardTitle
          >
          <VCardText>
            <VRow>
              <VCol md="12" cols="12">
                <strong>Pull Request:</strong><span>{{ branch.name }}</span>
              </VCol>
            </VRow>
            <VRow>
              <VCol md="4" cols="4" v-if="branch.isClosedToMaster">
                <strong style="color: greenyellow"> Закрыт</strong>
              </VCol>
              <VCol md="4" cols="4" v-else>
                <strong style="color: red"> Не закрыт</strong>
              </VCol>
              <VCol md="4" cols="4">
                <VBtn
                  v-if="!branch.hasPullRequest && !branch.isClosedToMaster"
                  @click="
                    createPull(
                      store.project.projectKey,
                      store.projectRepository.slug,
                      branch.name
                    )
                  "
                  >Создать PULL</VBtn
                >
              </VCol>
              <VCol md="4" cols="4">
                <VBtn
                  v-if="!branch.isClosedToMaster"
                  @click="
                    mergePull(
                      store.project.projectKey,
                      store.projectRepository.slug,
                      branch.pullRequest.id,
                      branch.name
                    )
                  "
                  >Закрыть</VBtn
                >
              </VCol>
            </VRow>
          </VCardText>
        </VCard>
      </VCol>
    </VRow>
  </VContainer>
</template>
