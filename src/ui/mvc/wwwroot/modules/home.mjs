const cardElements = document.querySelectorAll(".link-card");

cardElements.forEach((cardElement) => {
    setCardImage(cardElement);
});

export function setCardImage(card) {
    if (!card) return;

    const img = card.querySelector("img");
    const dataSrc = img.getAttribute("data-src");
    const noImage = "./imgs/no-image-found.png";

    let newSrc = noImage;

    if (dataSrc) {
        if (dataSrc.startsWith('http')) {
            if (dataSrc.indexOf('duckduckgo') >= 0) {
                newSrc = '/duckduckgo-logo.jpg';
            }
            else if (dataSrc.indexOf('google') >= 0) {
                newSrc = '/google-logo.jpg';
            }
            else if (dataSrc.indexOf('reddit') >= 0) {
                newSrc = '/reddit-logo.png';
            }
        }
        else {
            newSrc = dataSrc;
        }  
    }

    img.setAttribute("src", newSrc);

    img.onerror = function () {
        ///console.error(`Error loading image from ${src}`);
        console.log(`Error loading image from ${dataSrc}`);

        img.setAttribute("src", noImage);
    };   

    //img.setAttribute("data-src", "");

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
