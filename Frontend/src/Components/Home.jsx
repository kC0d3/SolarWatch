import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Home({ loggedUser }) {
    const [cityData, setCityData] = useState(undefined);
    const [citySearch, setCitySearch] = useState("");
    const [solarData, setSolarData] = useState(undefined);
    const [solarSearch, setSolarSearch] = useState({ city: '', date: '' });
    const loggedUserRole = loggedUser && JSON.parse(atob(loggedUser.token.split('.')[1]))['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const navigate = useNavigate();

    const fetchSolar = async () => {
        try {
            const response = await fetch(`/api/solars/${solarSearch.city}&${solarSearch.date}`, {
                method: 'GET',
                headers: {
                    authorization: `Bearer ${loggedUser.token}`
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            setSolarData(data);
        } catch (error) {
            console.error('Fetch error:', error);
            setSolarData(undefined);
        }
    };

    const fetchPostSolar = async () => {
        try {
            const response = await fetch(`/api/solars/?city=${solarSearch.city}&date=${solarSearch.date}`, {
                method: 'POST',
                headers: {
                    authorization: `Bearer ${loggedUser.token}`
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            setSolarData(data);
        } catch (error) {
            console.error('Fetch error:', error);
            setSolarData(undefined);
        }
    }

    const fetchCity = async () => {
        try {
            const response = await fetch(`/api/cities/${citySearch}`, {
                method: 'GET',
                headers: {
                    authorization: `Bearer ${loggedUser.token}`
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            setCityData(data);
        } catch (error) {
            console.error('Fetch error:', error);
            setCityData(undefined);
        }
    };

    const AddCity = async searchData => {
        if (loggedUser) {
            try {
                const response = await fetch(`/api/cities?city=${searchData}`, {
                    method: 'POST',
                    headers: {
                        authorization: `Bearer ${loggedUser.token}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();
                setCityData(data);
            } catch (error) {
                console.error('Fetch error:', error);
                setCityData(undefined);
            }
        }
    }

    const AddSolar = async () => {
        if (loggedUser) {
            try {
                const response = await fetch(`/api/solars/?city=${solarSearch.city}&date=${solarSearch.date}`, {
                    method: 'POST',
                    headers: {
                        authorization: `Bearer ${loggedUser.token}`
                    }
                });

                if (!response.ok) {
                    if (response.status === 404) {
                        await AddCity(solarSearch.city);
                        setCityData(undefined);
                        await fetchPostSolar();
                    } else {
                        throw new Error('Network response was not ok');
                    }
                } else {
                    const data = await response.json();
                    setSolarData(data);
                }
                fetchCity();
            } catch (error) {
                console.error('Fetch error:', error);
                setSolarData(undefined);
                setCityData(undefined);
            }
        }
    }

    useEffect(() => {
        if (loggedUser) {
            fetchSolar();
            fetchCity();
        } else {
            navigate('/login');
        }
    }, [loggedUser, solarSearch, citySearch]);

    return (
        <>
            <div>Looking for solar data:</div>
            <br></br>
            <input type='text' onChange={e => setSolarSearch({ ...solarSearch, city: e.target.value })} value={solarSearch.city} />
            <input type='date' onChange={e => setSolarSearch({ ...solarSearch, date: e.target.value })} value={solarSearch.date} />
            {loggedUserRole == 'Admin' && <button onClick={() => AddSolar()}>Add</button>}
            <br></br>
            {solarData ?
                <>
                    <div>City: {solarData.city.name}</div>
                    <div>Country: {solarData.city.country}</div>
                    <div>Latitude: {solarData.city.latitude}</div>
                    <div>Longitude: {solarData.city.longitude}</div>
                    <div>Sunrise: {solarData.sunrise}</div>
                    <div>Sunset: {solarData.sunset}</div>
                    <div>Date: {solarData.date}</div>
                    <br></br>
                </> : <><div>Not found!</div><br></br></>}
            <div>Looking for city data:</div>
            <br></br>
            <input type='text' onChange={e => setCitySearch(e.target.value)} value={citySearch} />
            {loggedUserRole == 'Admin' && <button onClick={() => AddCity(citySearch)}>Add</button>}
            <br></br>
            {cityData ?
                <>
                    <div>City: {cityData.name}</div>
                    <div>Country: {cityData.country}</div>
                    <div>Latitude: {cityData.latitude}</div>
                    <div>Longitude: {cityData.longitude}</div>
                    <br></br>
                    {cityData.solars.map(solar => (
                        <div key={solar.id}>
                            <div>Sunrise: {solar.sunrise}</div>
                            <div>Sunset: {solar.sunset}</div>
                            <div>Date: {solar.date}</div>
                            <br></br>
                        </div>
                    ))}

                    <br></br>
                </> : <><div>Not found!</div><br></br></>}
        </>
    );
}
