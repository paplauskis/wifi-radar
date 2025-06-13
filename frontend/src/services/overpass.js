const API_URL = 'http://localhost:44304/api/map'

/**
 * @param {number} lat - Center latitude
 * @param {number} lon - Center longitude
 * @param {number} radiusKm - Radius in kilometers (default 2km)
 * @returns {Promise<object[]>} List of nodes with WiFi
 */
export async function fetchWifiNodes(lat, lon, radiusKm = 2) {
  const radiusMeters = radiusKm * 1000

  const url = new URL(`${API_URL}/search`)
  url.searchParams.set('city', 'Vilnius') // TODO: Get city from coordinates
  url.searchParams.set('radius', radiusMeters.toString())

  const res = await fetch(url.toString(), {
    headers: {
      'User-Agent': 'wifi-radar-collage-project/1.0',
    },
  })

  if (!res.ok) {
    return []
  }

  const data = await res.json()
  return data.data || []
}
