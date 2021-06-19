// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddToCart(productId) {
    fetch("https://localhost:44343/cart/addtocart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
            IncreaseProductQuantity(productId);
            CalculateTotalPrice();
        });
}
function ReduceFromCart(productId) {
    fetch("https://localhost:44343/cart/reducefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            console.log(data);
            document.getElementById("cartButtonCount").innerHTML = data;
            DecreaseProductQuantity(productId);
            CalculateTotalPrice();
        });
}
function RemoveFromCart(productId) {
    fetch("https://localhost:44343/cart/removefromcart/?productId=" + productId)
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
            HideRemovedProduct(productId);
            CalculateTotalPrice();
        });
}

function CountProductsInCart() {
    fetch("https://localhost:44343/cart/countproductsincart")
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("cartButtonCount").innerHTML = data;
        });
}

function IncreaseProductQuantity(productId) {
    let productQuantityElement = document.getElementById('productinCartQuantity_' + productId);
    productQuantityElement.innerHTML = parseInt(productQuantityElement.innerHTML) + 1;

}

function DecreaseProductQuantity(productId) {
    let productQuantityElement = document.getElementById('productinCartQuantity_' + productId);
    let quantity = parseInt(productQuantityElement.innerHTML) - 1;
    productQuantityElement.innerHTML = quantity;

    HideProductLessThanOne(productId, quantity);

}

function HideProductLessThanOne(productId, quantity) {
    if (quantity < 1) {
        HideRemovedProduct(productId);
    }

}

function HideRemovedProduct(productId) {
    document.getElementById('productInCart_' + productId).style.display = 'none';
}

function CalculateTotalPrice() {
    fetch("https://localhost:44343/cart/calculatetotalprice/")
        .then(cartResponse => {
            if (cartResponse.ok) {
                return cartResponse.text();
            }
        }).then(data => {
            document.getElementById("totalPrice").innerHTML = data;
        });

}

window.onload = (event) => {
    CountProductsInCart();
}


var slideIndex = 0;
showSlides();

function showSlides() {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slideIndex++;
    if (slideIndex > slides.length) { slideIndex = 1 }
    slides[slideIndex - 1].style.display = "block";
    setTimeout(showSlides, 2000); // Change image every 2 seconds
}