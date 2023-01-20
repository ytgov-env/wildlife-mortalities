window.mortality =
{
    mapper: {},

    initSignaturePad: function (id) {
        const canvas = document.getElementById(id);

        mortality.mapper[id] = new SignaturePad(canvas);
    },

    resetSignaturePad: function (id) {

        const canvas = mortality.mapper[id];
        if (canvas) {
            canvas.clear();
        }
    },

    getSignaturePadContent: function (id) {

        const canvas = mortality.mapper[id];
        if (canvas) {
            return canvas.toDataURL();
        }

        return "";
    },

    removeSignaturePad: function (id) {
        delete mortality.mapper[id];
    }
};
