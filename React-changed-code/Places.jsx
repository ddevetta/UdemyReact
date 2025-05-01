import { useEffect, useRef } from "react";
import global from "../../GlobalVariables.js";

export default function Places({ title, places, fallbackText, onSelectPlace, isLoading, loadingText }) {
  if (!places) 
    places = [];
  return (
    <section className="places-category">
      <h2>{title}</h2>
      {isLoading && <p className="fallback-text">{loadingText}</p>}
      {!isLoading && places.length === 0 && <p className="fallback-text">{fallbackText}</p>}
      {!isLoading && places.length > 0 && (
        <ul className="places">
          {places.map((place, index) => (
            <li id={place.id} key={place.id} className="place-item">
              <button onClick={() => onSelectPlace(place)}>
                <img src={`${global.serverBaseUrl}/images/${place.image.src}`} alt={place.image.alt} />
                <h3>{place.title}</h3>
              </button>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
