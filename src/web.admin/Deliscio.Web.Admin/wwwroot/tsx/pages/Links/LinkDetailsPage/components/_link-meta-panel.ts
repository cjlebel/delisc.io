import { initElement } from "../../../../utils/dom/init-element";

export class LinkMetaPanel {

    panel!: HTMLDivElement;

    imgLinkImage!: HTMLImageElement;

    txtDateSubmitted!: HTMLInputElement;
    txtDateUpdated!: HTMLInputElement;
    txtSubmittedBy!: HTMLInputElement;
    txtCountLikes!: HTMLInputElement;
    txtCountSaves!: HTMLInputElement;

    txtMetaKeywords!: HTMLInputElement;

    /**
     * Swaps out the data-src for some known links, sucj as Google, for a nicer version.
     * @param images An array of Image Elements
     * @returns Nothing
     * @remarks: Copied from website
     */
    private cleanseImage = (image: HTMLImageElement) => {

        if (!image)
            return;

        const noImgFound = '/imgs/no-image-found.png';
        let newSrc = '';

        if (!image.src || !image.getAttribute("data-src")) {
            image.setAttribute("src", noImgFound);
            image.alt = "Image has no src or data-src";

            return;
        }

        const dataSrc = image.getAttribute("data-src") ?? '';

        // Covers both http: and https:
        if (dataSrc.startsWith('http')) {

            if (dataSrc.indexOf('duckduckgo') >= 0) {

                newSrc = '/imgs/duckduckgo-logo.jpg';

            }
            else if (dataSrc.indexOf('google') >= 0) {

                newSrc = '/imgs/google-logo.jpg';

            }
            else if (dataSrc.indexOf('reddit') >= 0) {

                newSrc = '/imgs/reddit-logo.png';

            }
            else {

                newSrc = dataSrc;

            }
        }

        try {
            image.setAttribute("src", newSrc);
        } catch (err) {

            console.error(`Error loading image from ${dataSrc}\n${err}`);
            image.setAttribute("src", noImgFound);

        }

    }

    constructor() {
        this.initForm();
    }

    private initForm(): void {
        this.panel = initElement(document, '#link-meta-details');

        this.imgLinkImage = initElement(this.panel, '#link-image');

        this.txtDateSubmitted = initElement(this.panel, '#date-submitted');
        this.txtDateUpdated = initElement(this.panel, '#date-updated');
        this.txtSubmittedBy = initElement(this.panel, '#submitted-by');
        this.txtCountLikes = initElement(this.panel, '#likes');
        this.txtCountSaves = initElement(this.panel, '#saves');
        this.txtMetaKeywords = initElement(this.panel, '#keywords');

        this.cleanseImage(this.imgLinkImage);
    }

}