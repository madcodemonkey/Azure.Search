import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import IndexStore from "./indexStore";

interface Store {
    commonStore: CommonStore;
    indexStore: IndexStore;
    modalStore: ModalStore
}

export const store: Store = {
    commonStore: new CommonStore(),
    indexStore: new IndexStore(),
    modalStore: new ModalStore()
}

// putting our store into the React context for retrieval 
export const StoreContext = createContext(store);

// a hook
export function useStore() {
    return useContext(StoreContext);
}