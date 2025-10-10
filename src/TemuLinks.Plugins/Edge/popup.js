document.addEventListener("DOMContentLoaded", function () {
  const saveButton = document.getElementById("saveButton");
  const descriptionInput = document.getElementById("description");
  const isPublicCheckbox = document.getElementById("isPublic");
  const statusDiv = document.getElementById("status");
  const configLink = document.getElementById("configLink");

  // Aktiven Tab lesen und Beschreibung vorbelegen
  chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
    const currentTab = tabs[0];
    const url = (currentTab.url || "").toLowerCase();
    const title = currentTab.title || "";
    // Beschreibung mit dem Tab-Titel vorbelegen
    const descEl = document.getElementById("description");
    if (descEl && !descEl.value) {
      descEl.value = title;
    }

    if (!url.includes("temu.com")) {
      showStatus("Diese Erweiterung funktioniert nur auf Temu.com", "error");
      saveButton.disabled = true;
    }
  });

  // Save button click handler
  saveButton.addEventListener("click", async function () {
    try {
      saveButton.disabled = true;
      saveButton.textContent = "Speichere...";

      // Get current tab
      const tabs = await chrome.tabs.query({
        active: true,
        currentWindow: true,
      });
      const currentTab = tabs[0];

      if (!(currentTab.url || "").toLowerCase().includes("temu.com")) {
        showStatus("Diese Erweiterung funktioniert nur auf Temu.com", "error");
        return;
      }

      // Get API configuration
      const config = await chrome.storage.sync.get(["apiEndpoint", "apiKey"]);

      if (!config.apiEndpoint || !config.apiKey) {
        showStatus("Bitte zuerst die API-Einstellungen konfigurieren", "error");
        configLink.click();
        return;
      }

      // Prepare data
      const linkData = {
        url: currentTab.url,
        description: descriptionInput.value.trim(),
        isPublic: isPublicCheckbox.checked,
      };

      // Send to API
      const response = await fetch(
        `${config.apiEndpoint.replace(/\/$/, "")}/api/temulinks`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "X-API-Key": config.apiKey,
          },
          body: JSON.stringify(linkData),
        }
      );

      if (response.ok) {
        showStatus("Link wurde erfolgreich gespeichert", "success");
        descriptionInput.value = "";
        isPublicCheckbox.checked = false;
      } else {
        const errorData = await response.json();
        showStatus(
          `Fehler: ${errorData.message || "Speichern fehlgeschlagen"}`,
          "error"
        );
      }
    } catch (error) {
      showStatus(`Fehler: ${error.message}`, "error");
    } finally {
      saveButton.disabled = false;
      saveButton.textContent = "Link speichern";
    }
  });

  // Klick auf Konfigurations-Link
  configLink.addEventListener("click", function (e) {
    e.preventDefault();
    chrome.runtime.openOptionsPage();
  });

  function showStatus(message, type) {
    statusDiv.textContent = message;
    statusDiv.className = `status ${type}`;
    statusDiv.style.display = "block";

    setTimeout(() => {
      statusDiv.style.display = "none";
    }, 3000);
  }
});

