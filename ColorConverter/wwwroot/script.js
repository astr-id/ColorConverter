let token = "";

async function login() {
    const user = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value
    };

    const response = await fetch("/api/login/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    const authDiv = document.getElementById("authStatus");

    if (response.ok) {
        const data = await response.json();
        token = data.token;
        authDiv.innerHTML = `<div class="alert alert-success">Connecté !</div>`;
    } else {
        authDiv.innerHTML = `<div class="alert alert-danger">Identifiants invalides</div>`;
    }
}

async function convertColor() {
    const payload = {
        inputFormat: document.getElementById("inputFormat").value,
        outputFormat: document.getElementById("outputFormat").value,
        colorValue: document.getElementById("colorValue").value
    };

    const resultDiv = document.getElementById("conversionResult");

    const response = await fetch("/api/color/convert", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        const data = await response.json();
        resultDiv.innerHTML = `<strong>Résultat :</strong><br>${data.result}`;

        // Appliquer couleur si le format est utilisable en CSS
        const previewBox = document.getElementById("colorPreview");
        const cssColor = parseToCssColor(data.result);

        if (cssColor) {
            previewBox.style.backgroundColor = cssColor;
        } else {
            previewBox.style.backgroundColor = "transparent";
        }
    } else {
        resultDiv.innerHTML = `<div class="alert alert-danger">Erreur de conversion.</div>`;
    }
}

// Fonction utilitaire déclarée en dehors
function parseToCssColor(value) {
    const hexRegex = /^#([A-Fa-f0-9]{6})$/;
    const rgbRegex = /^rgb\(\d+,\s*\d+,\s*\d+\)$/;

    if (hexRegex.test(value.trim())) return value.trim();
    if (rgbRegex.test(value.trim())) return value.trim();

    return null;
}
// Gère le clic sur la box couleur
document.getElementById("colorPreview").addEventListener("click", () => {
    const resultText = document.getElementById("conversionResult").innerText;
    if (!resultText) return;

    const valueToCopy = resultText.split(":")[1]?.trim();
    if (!valueToCopy) return;

    navigator.clipboard.writeText(valueToCopy).then(() => {
        const box = document.getElementById("colorPreview");
        box.classList.add("copied");

        setTimeout(() => {
            box.classList.remove("copied");
        }, 1500);
    });
});
