const NOMINATIM_URL = 'https://nominatim.openstreetmap.org/search'

/**
 * @param {string} city - City name
 * @returns {Promise<object|null>} First matching result
 */

export async function geocodeCity(city) {
  const url = new URL(NOMINATIM_URL)
  url.searchParams.set('q', city)
  url.searchParams.set('format', 'json')
  url.searchParams.set('limit', '1')
  url.searchParams.set('addressdetails', '1')

  const res = await fetch(url.toString(), {
    headers: {
      'User-Agent': 'wifi-radar-collage-project/1.0',
    },
  })

  const data = await res.json()
  return data[0] || null
}
