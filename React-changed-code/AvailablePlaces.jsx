import { useState, useEffect } from 'react';

import Places from './Places.jsx';
import ErrorPage from './Error.jsx';
import { sortPlacesByDistance } from '../loc.js';
import { fetchAvailablePlaces } from '../http.js';

export default function AvailablePlaces({ onSelectPlace }) {
  
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState();
  const [availablePlaces, setAvailablePlaces] = useState([]);

  useEffect(() => {
    async function getPlaces() {

      try {
        setIsFetching(true);

        const payload = await fetchAvailablePlaces();

        navigator.geolocation.getCurrentPosition((position) => {
          const sortedPlaces = sortPlacesByDistance(payload.places, position.coords.latitude, position.coords.longitude);
          setAvailablePlaces(sortedPlaces);
          setIsFetching(false);
        })
      }
      catch (error) {
        setError(error)
        setIsFetching(false);
      }
    }
    getPlaces();

    // fetch ("http://localhost:3000/places")
    //   .then((response) => {
    //     return response.json();
    //   })
    //   .then((payload) => {
    //     setAvailablePlaces(payload.places);
    //   });
  }, []);

  if (error) {
    return <ErrorPage title="An error occurred." message={error.message} />
  }

  return (
    <Places
      title="Available Places"
      places={availablePlaces}
      fallbackText="No places available."
      isLoading={isFetching}
      loadingText="Fetching places..."
      onSelectPlace={onSelectPlace}
    />
  );
}
