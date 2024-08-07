import { StyleSheet, View, Text, FlatList, SafeAreaView, Pressable } from 'react-native';
import { useState, useEffect } from 'react';
import { useNavigation } from "expo-router";

import { COLORS } from '../constants/theme';
import { apiUrl } from "../constants/config";
import PartyCard from '../components/partyCard';
import Logo from '../components/logo';
import Loading from '../components/loading';
import LogOut from '../components/logout';

export async function fetchParties() {
    const res = await fetch(`${apiUrl}Party/GetParties`,
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
    await fetch(`${apiUrl}Party/DeleteParty?partyId=${id}`,
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
            <View style={styles.fixedContainer}>
                <LogOut />
                <Logo />
                <View style={styles.header}>
                    <Text style={styles.main}>Bulik</Text>
                </View>
                <View style={styles.buttonContainer}>
                    <Pressable onPress={() => navigation.navigate('createParty')} style={styles.button}>
                        <Text style={styles.buttonText}>Buli létrehozás</Text>
                    </Pressable>
                </View>
            </View>
            <View style={styles.listContainer}>
                {parties.length === 0 ? (
                    <View style={styles.messageContainer}>
                        <Text style={styles.message}>Nincsenek még bulik. Hozz létre egyet!</Text>
                    </View>
                ) : (<FlatList
                    data={parties}
                    renderItem={({ item }) => <PartyCard party={item} onPress={() => navigation.navigate('songRequests', { id: item.id })} onDelete={() => onDelete(item.id, setParties, setLoading)} />}
                    keyExtractor={item => item.id}
                    contentContainerStyle={styles.listContent}
                />)}
            </View>
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    safeArea: {
        flex: 1,
    },
    header: {
        padding: 16,
        backgroundColor: COLORS.black,
        alignItems: 'center',
        width: '110%',
    },
    fixedContainer: {
        alignItems: 'center',
        justifyContent: 'flex-start',
        padding: 12,
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
        justifyContent: 'center',
        alignItems: 'center',
        marginTop: 20,
        marginBottom: 10,
    },
    button: {
        justifyContent: 'center',
        alignItems: 'center',
        paddingVertical: 8,
        paddingHorizontal: 16,
        elevation: 2,
        width: 200,
        backgroundColor: COLORS.ashAndCreme,
        borderWidth: 1,
        borderColor: COLORS.ashAndCreme,
        borderRadius: 5,
    },
    buttonText: {
        fontSize: 16,
        fontWeight: 'bold',
        color: COLORS.white,
    },
    messageContainer: {
        alignItems: 'center'
    },
    message: {
        fontSize: 20,
        fontWeight: 'bold',
        color: '#000',
    },
    listContainer: {
        flex: 1,
        marginTop: 16,
    },
});