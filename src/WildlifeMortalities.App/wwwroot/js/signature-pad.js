window.signaturePad =
    {
        mapper: {},

        initSignaturePad: function (id) {
            const canvas = document.getElementById(id);

            signaturePad.mapper[id] = new SignaturePad(canvas);
        },

        resetSignaturePad: function (id) {

            const canvas = signaturePad.mapper[id];
            if (canvas) {
                canvas.clear();
            }
        },

        getSignaturePadContent: function (id) {

            const canvas = signaturePad.mapper[id];
            if (canvas) {
                return canvas.toDataURL();
            }

            return "";
        },

        removeSignaturePad: function (id) {
            delete signaturePad.mapper[id];
        }
    };
