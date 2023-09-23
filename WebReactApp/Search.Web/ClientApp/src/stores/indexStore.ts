import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { IndexData } from "../models/indexData";

export default class IndexStore {
    indexList: IndexData[] = [];
    initialized = false;
    loading = false;

    constructor() {
        makeAutoObservable(this);
    }

    /**
    * Clears out the store and resets it to the state it was in when it was first started.
    */
    clear() {
        this.indexList = [];
        this.initialized = false;
    }

    /**
     * Loads index data from the server.
     * @returns 
     */
    loadIndexes = async (): Promise<IndexData[]> => {
        if (this.initialized) return this.indexList;
        this.loading = true;
        try {
            this.indexList = await agent.ActivityApi.getAll();

            runInAction(() => {
                this.loading = false;
                this.initialized = true;
            });

            return this.indexList;
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            });
        }

        return [];
    } 
}