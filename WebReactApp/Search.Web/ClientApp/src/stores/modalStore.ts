import { makeAutoObservable } from "mobx"
import { ReactNode } from "react";

interface Modal {
    open: boolean;
    body: ReactNode | null; // was JsxElement
}

export default class ModalStore {
    modal: Modal = {
        open: false,
        body: null
    }

    constructor() {
        makeAutoObservable(this);
    }

    openModal = (content: ReactNode) => {
        this.modal.open = true;
        this.modal.body = content;
    }

    closeModal = () => {
        this.modal.open = false;
        this.modal.body = null;
    }
}