document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("configForm");
  const apiEndpointInput = document.getElementById("apiEndpoint");
  const apiKeyInput = document.getElementById("apiKey");
  const testButton = document.getElementById("testButton");
  const statusDiv = document.getElementById("status");

  // Load saved configuration
  chrome.storage.sync.get(["apiEndpoint", "apiKey"], function (result) {
    if (result.apiEndpoint) {
      apiEndpointInput.value = result.apiEndpoint;
    }
    if (result.apiKey) {
      apiKeyInput.value = result.apiKey;
    }
  });

  // Form submit handler
  form.addEventListener("submit", function (e) {
    e.preventDefault();
    saveConfiguration();
  });

  // Test connection button
  testButton.addEventListener("click", function () {
    testConnection();
  });

  function saveConfiguration() {
    const apiEndpoint = apiEndpointInput.value.trim();
    const apiKey = apiKeyInput.value.trim();

    if (!apiEndpoint || !apiKey) {
      showStatus("Bitte alle Felder ausfüllen", "error");
      return;
    }

    chrome.storage.sync.set(
      {
        apiEndpoint: apiEndpoint,
        apiKey: apiKey,
      },
      function () {
        showStatus("Die Konfiguration wurde gespeichert", "success");
      }
    );
  }

  async function testConnection() {
    let apiEndpoint = apiEndpointInput.value.trim();
    const apiKey = apiKeyInput.value.trim();

    if (!apiEndpoint || !apiKey) {
      showStatus("Bitte zuerst alle Felder ausfüllen", "error");
      return;
    }

    try {
      testButton.disabled = true;
      testButton.textContent = "Teste...";

      // Normalize base URL (remove trailing slash)
      if (apiEndpoint.endsWith("/")) {
        apiEndpoint = apiEndpoint.slice(0, -1);
      }

      const response = await fetch(`${apiEndpoint}/api/temulinks/count`, {
        method: "GET",
        headers: {
          "X-API-Key": apiKey,
        },
      });

      if (response.ok) {
        const data = await response.json();
        const count = data.count ?? data.Count ?? "unbekannt";
        showStatus(
          `Verbindung erfolgreich! Du hast ${count} gespeicherte Links.`,
          "success"
        );
      } else {
        showStatus(
          `Verbindung fehlgeschlagen: ${response.status} ${response.statusText}`,
          "error"
        );
      }
    } catch (error) {
      showStatus(`Verbindungsfehler: ${error.message}`, "error");
    } finally {
      testButton.disabled = false;
      testButton.textContent = "Verbindung testen";
    }
  }

  function showStatus(message, type) {
    statusDiv.textContent = message;
    statusDiv.className = `status ${type}`;
    statusDiv.style.display = "block";

    setTimeout(() => {
      statusDiv.style.display = "none";
    }, 5000);
  }
});


