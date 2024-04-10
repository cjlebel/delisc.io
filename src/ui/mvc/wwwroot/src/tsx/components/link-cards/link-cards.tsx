import LinkCard from './link-card';

//import NoImage from '../../../../imgs/no-image-found.png';

export default function LinkCards() {

    console.log('LinkCards loaded');

    const cards = Array.from(document.querySelectorAll(".link-card"));

    if (!cards)
        return;

    cards.forEach(card => {

        LinkCard(card);

    });

    const images = Array.from(document.querySelectorAll('.link-card img'));

    if (images)
        cleanseImages(images);


    /**
     * Swaps out the data-src for some known links, sucj as Google, for a nicer version.
     * @param images An array of Image Elements
     * @returns Nothing
     */
    function cleanseImages(images: Element[]) {

        console.log('Cleansing images');

        if (!images)
            return;

        console.log(`Cleansing ${images.length} images`);

        let newSrc = "";// NoImage;

        images.forEach(image => {

            if (!image?.getAttribute("data-src"))
                return;

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

            image.setAttribute("data-src", newSrc);
            /*image.setAttribute("src", newSrc);*/

        });

    }

};