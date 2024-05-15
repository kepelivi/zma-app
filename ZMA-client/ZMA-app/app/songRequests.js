import { View, Text, Pressable, StyleSheet, SafeAreaView, FlatList } from 'react-native';
import { useEffect, useState } from 'react';
import { useRoute } from '@react-navigation/native';

import SongCard from '../components/songCard';
import Logo from '../components/logo';
import { COLORS } from '../constants/theme';

export default function songRequests() {
    const [songs, setSongs] = useState([]);
    const [loading, setLoading] = useState(true);

    const route = useRoute();
    const { params } = route;

    async function fetchSongs() {
        const res = await fetch(`http://localhost:5086/Party/GetSongs?partyId=${params.id}`,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            });
        return await res.json();
    }

    useEffect(() => {
        fetchSongs()
        .then(songs => {
            setSongs(songs);
            setLoading(false);
        })
    }, []);

    if (loading) return <View><Text>Loading...</Text></View>

    return (
        <SafeAreaView style={styles.safeArea}>
            <Logo />
            <View style={styles.header}>
                <Text style={styles.main}>Kért zenék</Text>
            </View>
            <FlatList
                data={songs}
                renderItem={({ item }) => <SongCard song={item} />}
                keyExtractor={item => item.id}
                contentContainerStyle={styles.listContent}
            />
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
  });