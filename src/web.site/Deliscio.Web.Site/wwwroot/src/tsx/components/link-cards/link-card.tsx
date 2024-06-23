export default function LinkCard(card: Element) {

    console.log('LinkCard loaded');

    if (!card)
        return;

    const img = card.querySelector("img");

    if (img) {

        const cardWidth = card.clientWidth;
        const cardHeight = card.clientHeight;
        const imgWidth = img.clientWidth;
        const imgHeight = img.clientHeight;
        const imgRatio = imgWidth / imgHeight;
        const cardRatio = cardWidth / cardHeight;

        if (imgRatio > cardRatio) {
            img.style.width = "100%";
            img.style.height = "auto";
        } else {
            img.style.width = "auto";
            img.style.height = "100%";
        }

    }

}