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
