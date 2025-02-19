import { ref, computed } from "vue";
import { defineStore } from "pinia";
import axios from "axios";

export declare namespace BitbucketData {
  export interface Project {
    name: string;
    projectKey: string;
    repositories: Repository[];
  }

  export interface Repository {
    name: string;
    id: number;
    link: string;
    slug: string;
    releaseBranches: Branch[];
  }

  export interface Branch {
    name: string;
    name: string;
    lastCommitHash: string;
    hasPullRequest: boolean;
    lastCommitDate: Date;
    isClosedToMaster: boolean;
    pullrequest: PullRequest;
  }

  export interface PullRequest {
    slug: string;
    projectKey: string;
    branchName: string;
    id: number;
  }
}

export const useBitbucketDataStore = defineStore("BitbucketData", {
  state: () => ({
    $projects: null,
    $project: null,
    $projectRepository: null,
  }),
  getters: {
    projects(): Project[] {
      return this.$projects;
    },
    project(): Project {
      return this.$project;
    },
    projectRepository(): Repository {
      return this.$projectRepository;
    },
  },
  actions: {
    async fetchProjects(): Promise<void> {
      var res = await axios.get<Project[]>(
        "https://localhost:7054/api/bitbucket/projects"
      );
      this.$projects = res.data;
    },
    async fetchProject(key: string): Promise<void> {
      var res = await axios.get<Project>(
        `https://localhost:7054/api/bitbucket/project/${key}`
      );
      this.$project = res.data;
    },
    async fetchRepository(key: string, slug: string): Promise<void> {
      var res = await axios.get<Repository>(
        `https://localhost:7054/api/bitbucket/repository/${key}/${slug}`
      );

      this.$projectRepository = res.data;
    },
    async createPullRequest(
      key: string,
      slug: string,
      branchName: string
    ): Promise<void> {
      await axios.post(
        `https://localhost:7054/api/bitbucket/pullrequest/create`,
        {
          ProjectKey: key,
          Slug: slug,
          BranchName: branchName,
        }
      );
    },
    async mergePullRequest(
      key: string,
      slug: string,
      id: number,
      branchName: string
    ): Promise<void> {
      await axios.post(
        `https://localhost:7054/api/bitbucket/pullrequest/merge`,
        {
          ProjectKey: key,
          Slug: slug,
          Id: id,
          BranchName: branchName,
        }
      );
    },
  },
});
