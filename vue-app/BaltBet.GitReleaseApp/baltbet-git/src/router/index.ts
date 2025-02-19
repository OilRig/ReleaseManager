import { createRouter, createWebHistory } from "vue-router";
import ProjectsView from "../views/ProjectsView.vue";
import { useBitbucketDataStore } from "../stores/bitbucket";
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/projects",
      name: "projects",
      component: ProjectsView,
      async beforeEnter(routeTo) {
        const store = useBitbucketDataStore();

        await store.fetchProjects();
      },
    },
    {
      path: "/project/:key",
      name: "project",
      async beforeEnter(routeTo) {
        const store = useBitbucketDataStore();
        await store.fetchProject(routeTo.params.key as string);
      },
      component: () => import("../views/ProjectView.vue"),
    },
    {
      path: "/repository/:key/:slug",
      name: "repository",
      async beforeEnter(routeTo) {
        const store = useBitbucketDataStore();
        await store.fetchRepository(
          routeTo.params.key as string,
          routeTo.params.slug as string
        );
      },
      component: () => import("../views/RepositoryView.vue"),
    },
  ],
});

export default router;
