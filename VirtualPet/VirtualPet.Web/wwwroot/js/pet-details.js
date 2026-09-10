document.addEventListener("DOMContentLoaded", () => {
    const forms = document.querySelectorAll(".js-pet-action");

    forms.forEach(form => {
        form.addEventListener("submit", handlePetAction);
    });
});

async function handlePetAction(event) {
    event.preventDefault(); //停止瀏覽器預設行為

    const form = event.currentTarget;

    const button = form.querySelector('button[type="submit"]');

    if (button) {
        button.disabled = true;
    }

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.detail ?? "操作失敗。");
        }

        updatePet(data.pet);

        if (data.evolved) {
            showMessage(`${data.message} 寵物進化成 ${data.pet.evolutionStage}!`, "success");
        }
        else {
            showMessage(data.message, "success");
        }

    }
    catch (error) {
        showMessage(error.message ?? "操作失敗。", "danger");
    }
    finally {
        if (button) {
            button.disabled = false;
        }
    }
}

function updatePet(pet) {
    document.getElementById("pet-level").textContent = pet.level;
    document.getElementById("pet-experience").textContent = pet.experience;
    document.getElementById("pet-state").textContent = pet.state;
    document.getElementById("pet-evolution-stage").textContent = pet.evolutionStage;

    updateStatus("pet-satiety", "pet-satiety-bar", pet.satiety);
    updateStatus("pet-happiness", "pet-happiness-bar", pet.happiness);
    updateStatus("pet-energy", "pet-energy-bar", pet.energy);

    document.getElementById("pet-last-status-update").textContent = formatUtcDate(pet.lastStatusUpdateAt);
}

function updateStatus(valueId, barId, value) {
    document.getElementById(valueId).textContent = value;
    document.getElementById(barId).style.width = `${value}%`;
}


function formatUtcDate(value) {
    const date = new Date(value);
    const year = date.getUTCFullYear();
    const moth = String(date.getUTCMonth() + 1).padStart(2, "0");
    const day = String(date.getUTCDate()).padStart(2, "0");
    const hour = String(date.getUTCHours()).padStart(2, "0");
    const minute = String(date.getUTCMinutes()).padStart(2, "0");
    const second = String(date.getUTCSeconds()).padStart(2, "0");

    return `${year}-${moth}-${day} ${hour}:${minute}:${second} UTC`;
}

function showMessage(message, type) {
    const messageElement = document.getElementById("pet-action-message");
    messageElement.textContent = message;
    messageElement.className = `alert alert-${type}`;
    messageElement.classList.remove("d-none");
}