import { View, Text, Pressable, StyleSheet, SafeAreaView, FlatList } from 'react-native';
import { useEffect, useState } from 'react';
import { useRoute } from '@react-navigation/native';

import SongCard from '../components/songCard';
import Logo from '../components/logo';
import { COLORS } from '../constants/theme';
import { apiUrl } from "../constants/config";
import Loading from '../components/loading';
import GoBack from '../components/back';

export default function songRequests() {
    const [songs, setSongs] = useState([]);
    const [loading, setLoading] = useState(true);

    const route = useRoute();
    const { params } = route;

    async function fetchSongs() {
        const res = await fetch(`${apiUrl}Song/GetSongs?partyId=${params.id}`,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        return await res.json();
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
            fetchAndSortSongs();
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
            fetchAndSortSongs();
        }
        catch (error) {
            console.log(error);
        }
    }

    async function fetchAndSortSongs() {
        try {
            const songs = await fetchSongs();

            const sortedSongs = songs.sort((a, b) => {
                return new Date(b.requestTime) - new Date(a.requestTime);
            });

            setSongs(sortedSongs);
        } catch (error) {
            console.error('Failed to fetch and sort songs', error);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        fetchAndSortSongs();
    }, []);

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
            ) : (<FlatList
                data={songs}
                renderItem={({ item }) => <SongCard song={item} onAccept={() => handleAccept(item.id)} onDeny={() => handleDeny(item.id)} />}
                keyExtractor={item => item.id}
                contentContainerStyle={styles.listContent}
            />)}
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