document.addEventListener("DOMContentLoaded", function () {
  const saveButton = document.getElementById("saveButton");
  const descriptionInput = document.getElementById("description");
  const isPublicCheckbox = document.getElementById("isPublic");
  const statusDiv = document.getElementById("status");
  const configLink = document.getElementById("configLink");

  // Load current tab URL and show info
  chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
    const currentTab = tabs[0];
    const url = (currentTab.url || "").toLowerCase();
    const title = currentTab.title || "";
    document.getElementById("pageUrl").textContent = currentTab.url || "n/a";
    document.getElementById("pageTitle").textContent = title;

    if (!url.includes("temu.com")) {
      showStatus("This extension only works on Temu.com", "error");
      saveButton.disabled = true;
    }
  });

  // Save button click handler
  saveButton.addEventListener("click", async function () {
    try {
      saveButton.disabled = true;
      saveButton.textContent = "Saving...";

      // Get current tab
      const tabs = await chrome.tabs.query({
        active: true,
        currentWindow: true,
      });
      const currentTab = tabs[0];

      if (!(currentTab.url || "").toLowerCase().includes("temu.com")) {
        showStatus("This extension only works on Temu.com", "error");
        return;
      }

      // Get API configuration
      const config = await chrome.storage.sync.get(["apiEndpoint", "apiKey"]);

      if (!config.apiEndpoint || !config.apiKey) {
        showStatus("Please configure API settings first", "error");
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
        showStatus("Link saved successfully!", "success");
        descriptionInput.value = "";
        isPublicCheckbox.checked = false;
      } else {
        const errorData = await response.json();
        showStatus(
          `Error: ${errorData.message || "Failed to save link"}`,
          "error"
        );
      }
    } catch (error) {
      showStatus(`Error: ${error.message}`, "error");
    } finally {
      saveButton.disabled = false;
      saveButton.textContent = "Save Link";
    }
  });

  // Config link click handler
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
