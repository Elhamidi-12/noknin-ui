const registrations = new WeakMap();

export function registerOutsideClick(rootElement, dotNetReference) {
    if (!rootElement) {
        return;
    }

    const handler = (event) => {
        if (!rootElement.contains(event.target)) {
            dotNetReference.invokeMethodAsync("CloseFromOutsideClickAsync");
        }
    };

    document.addEventListener("pointerdown", handler, true);

    registrations.set(rootElement, handler);
}

export function unregisterOutsideClick(rootElement) {
    if (!rootElement) {
        return;
    }

    const handler = registrations.get(rootElement);

    if (!handler) {
        return;
    }

    document.removeEventListener("pointerdown", handler, true);
    registrations.delete(rootElement);
}