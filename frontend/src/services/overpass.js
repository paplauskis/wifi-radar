const OVERPASS_URL = 'https://overpass-api.de/api/interpreter'

/**
 * @param {number} lat - Center latitude
 * @param {number} lon - Center longitude
 * @param {number} radiusKm - Radius in kilometers (default 2km)
 * @returns {Promise<object[]>} List of nodes with WiFi
 */
export async function fetchWifiNodes(lat, lon, radiusKm = 2) {
  const radiusMeters = radiusKm * 1000

  const query = `
    [out:json];
    node
      ["internet_access"="wlan"]
      (around:${radiusMeters},${lat},${lon});
    out body;
  `

  const res = await fetch(OVERPASS_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      'User-Agent': 'wifi-radar-collage-project/1.0',
    },
    body: `data=${encodeURIComponent(query)}`,
  })

  const data = await res.json()
  return data.elements || []
}
