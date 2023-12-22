const cardElements = document.querySelectorAll(".link-card");

cardElements.forEach((cardElement) => {
    setCardImage(cardElement);
});

export function setCardImage(card) {
    if (!card) return;

    const img = card.querySelector("img");
    const src = img.getAttribute("data-src");
    const noImage = "./imgs/no-image-found.png";

    if (src) {
        img.setAttribute("src", src);

        img.onerror = function () {
            //console.error(`Error loading image from ${src}`);
            console.log(`Error loading image from ${src}`);

            img.setAttribute("src", noImage);
        };
    }

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
