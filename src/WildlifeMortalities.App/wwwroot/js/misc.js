function preventEnterFromSubmitting(e) {
    if (e.code == 'Enter') {
        if (e.srcElement && e.srcElement.localName == 'button') {
            return true;
        }
        e.preventDefault();
        e.stopPropagation();
        return false;
    } else {
        return true;
    }
}

function isTestEnvironment() {
    const hostname = window.location.hostname;
    return hostname.includes("wildlifemortalities-test") || hostname.includes("localhost");
}