import global from '../GlobalVariables.js';

export async function fetchAvailablePlaces() {
    const response = await fetch (`${global.serverBaseUrl}/Place`); // C# server api/UdemyReact
    //const response = await fetch ("http://localhost:3000/places"); // node.js backend server

    if (!response.ok) {
        throw new Error("The request to the server failed.");
    }

    const resData = await response.json();
    return resData;
  }
  
export async function fetchUserPlaces() {
    const response = await fetch(`${global.serverBaseUrl}/userplace/${global.userId}`);
  
    if (!response.ok) {
      throw new Error('Failed to fetch user places');
    }

    const resData = await response.json();
    return resData;
  }
  
export async function addUserPlace(Place) {
    const payload = {
      method: 'POST',
      body: JSON.stringify({ Place }),
      headers: {
        'Content-Type': 'application/json',
      }
    };
    const response = await fetch(`${global.serverBaseUrl}/userplace/?user=${global.userId}&id=${Place.id}`, payload);

    if (!response.ok) {
      const msg = await response.json();
      if (msg && msg.source && msg.source == "UdemyReact")
        throw new Error(msg.title);
      
      throw new Error(`Received status ${response.status} from - ${response.url} - ${msg}`)
    }
  }

export async function deleteUserPlace(Place) {
    const payload = {
      method: 'DELETE',
      body: JSON.stringify({ Place }),
      headers: {
        'Content-Type': 'application/json',
      }
    };
    console.log(Place)
    const response = await fetch(`${global.serverBaseUrl}/userplace/?user=${global.userId}&id=${Place.id}`, payload);

    if (!response.ok) {
      const msg = await response.json();
      if (msg && msg.source && msg.source == "UdemyReact")
        throw new Error(msg.title);

      throw new Error(`Received status ${response.status} from - ${response.url} - ${msg}`)
    }
  }
  