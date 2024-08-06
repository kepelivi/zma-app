import { View, Text, Pressable, StyleSheet, SafeAreaView, FlatList } from 'react-native';
import { useEffect, useState } from 'react';
import { useRoute } from '@react-navigation/native';
import { HubConnectionBuilder } from '@microsoft/signalr';

import SongCard from '../components/songCard';
import Logo from '../components/logo';
import { COLORS } from '../constants/theme';
import { apiUrl } from "../constants/config";
import Loading from '../components/loading';
import GoBack from '../components/back';
import CollapsibleView from '../components/collapsableView';

export default function songRequests() {
    const [songs, setSongs] = useState([]);
    const [loading, setLoading] = useState(true);
    const [connection, setConnection] = useState(null);
    const [acceptedSongs, setAcceptedSongs] = useState([]);

    const route = useRoute();
    const { params } = route;

    async function fetchSongs() {
        const res = await fetch(`${apiUrl}Song/GetSongs?partyId=${params.id}`,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        const data = await res.json();

        const newSongs = [];
        const newAcceptedSongs = [];

        data.forEach(song => {
            song.accepted ? newAcceptedSongs.push(song) : newSongs.push(song);
        });

        setSongs(newSongs);
        setAcceptedSongs(newAcceptedSongs);

        return data;
    }

    async function setUpConnection() {
        const newConnection = new HubConnectionBuilder()
            .withUrl(`${apiUrl}songRequestHub`)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }

    async function handleAccept(songId) {
        try {
            const res = await fetch(`${apiUrl}Song/AcceptSong?songId=${songId}`,
                {
                    method: "PATCH",
                    credentials: 'include',
                    headers: { 'Content-type': 'application/json' }
                });
            if (!res.ok) {
                throw new Error("Something went wrong");
            }
            fetchSongs();
        }
        catch (error) {
            console.log(error);
        }
    }

    async function handleDeny(songId) {
        try {
            const res = await fetch(`${apiUrl}Song/DenySong?partyId=${params.id}&songId=${songId}`,
                {
                    method: "DELETE",
                    credentials: 'include',
                    headers: { 'Content-type': 'application/json' }
                });
            if (!res.ok) {
                throw new Error("Something went wrong");
            }
            fetchSongs();
        }
        catch (error) {
            console.log(error);
        }
    }

    useEffect(() => {
        fetchSongs()
            .then(() => {
                setLoading(false);
            })
        setUpConnection();
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    connection.on('receiveSongRequestUpdate', song => {
                        setSongs(prev => [...prev, song]);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    if (loading) return <Loading />

    return (
        <SafeAreaView style={styles.safeArea}>
            <GoBack />
            <Logo />
            <View style={styles.header}>
                <Text style={styles.main}>Kért zenék</Text>
            </View>
            {songs.length === 0 ? (
                <View style={styles.messageContainer}>
                    <Text style={styles.message}>Nincs még egy zenekérés sem.</Text>
                </View>
            ) : (<><FlatList
                data={songs}
                renderItem={({ item }) => <SongCard song={item} onAccept={() => handleAccept(item.id)} onDeny={() => handleDeny(item.id)} />}
                keyExtractor={item => item.id}
                contentContainerStyle={styles.listContent}
            />
                <CollapsibleView title="Elfogadott zenék" songs={acceptedSongs} />
            </>)}
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    safeArea: {
        flex: 1,
        backgroundColor: '#fff',
    },
    header: {
        padding: 16,
        backgroundColor: COLORS.deepPurple,
        alignItems: 'center',
    },
    main: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#fff',
    },
    listContent: {
        padding: 16,
    },
    messageContainer: {
        alignItems: 'center'
    },
    message: {
        fontSize: 20,
        fontWeight: 'bold',
        color: '#000',
    }
});