import { StyleSheet, View, Text, FlatList, SafeAreaView, Pressable } from 'react-native';
import { useState, useEffect } from 'react';
import { useNavigation } from "expo-router";

import { COLORS } from '../constants/theme';
import PartyCard from '../components/partyCard';
import Logo from '../components/logo';
import Loading from '../components/loading';
import LogOut from '../components/logout';

export async function fetchParties() {
    const res = await fetch(`http://localhost:5086/Party/GetParties`,
        {
            method: "GET",
            credentials: 'include',
            headers: { 'Content-type': 'application/json' }
        }
    );
    return await res.json();
}

export async function fetchAndSortParties(setParties, setLoading) {
    try {
        const parties = await fetchParties();
        const sortedParties = parties.sort((a, b) => new Date(a.date) - new Date(b.date));
        setParties(sortedParties);
    } catch (error) {
        console.error('Failed to fetch and sort parties', error);
    } finally {
        setLoading(false);
    }
};

export async function onDelete(id, setParties, setLoading) {
    await fetch(`http://localhost:5086/Party/DeleteParty?partyId=${id}`,
        {
            method: "DELETE",
            credentials: 'include',
            headers: { 'Content-type': 'application/json' }
        }
    );
    fetchAndSortParties(setParties, setLoading);
}

export default function PartyManager() {
    const [parties, setParties] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigation = useNavigation();

    useEffect(() => {
        fetchAndSortParties(setParties, setLoading);
    }, []);

    if (loading) return <Loading />

    return (
        <SafeAreaView style={styles.safeArea}>
            <LogOut />
            <Logo />
            <View style={styles.header}>
                <Text style={styles.main}>Bulik</Text>
            </View>
            <View style={styles.container}>
                <Pressable onPress={() => navigation.navigate('createParty')} style={[styles.button, { backgroundColor: '#9c27b0' }]}>
                    <Text style={styles.buttonText}>Buli létrehozás</Text>
                </Pressable>
            </View>
            <FlatList
                data={parties}
                renderItem={({ item }) => <PartyCard party={item} onPress={() => navigation.navigate('songRequests', { id: item.id })} onDelete={() => onDelete(item.id, setParties, setLoading)} />}
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
    buttonContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    button: {
        justifyContent: 'center',
        alignItems: 'center',
        paddingVertical: 8,
        paddingHorizontal: 16,
        elevation: 2,
    },
    buttonText: {
        fontSize: 16,
        fontWeight: 'bold',
        color: '#FFFFFF',
    },
});