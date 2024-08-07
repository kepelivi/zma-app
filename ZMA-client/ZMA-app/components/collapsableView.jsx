import React, { useState } from 'react';
import { View, Text, TouchableWithoutFeedback, Animated, StyleSheet, FlatList, Pressable, Image } from 'react-native';

import { COLORS } from '../constants/theme';

export default function CollapsibleView({ title, songs, onDelete }) {
    const [collapsed, setCollapsed] = useState(true);
    const [animation] = useState(new Animated.Value(0));

    const toggleCollapse = () => {
        if (collapsed) {
            Animated.timing(animation, {
                toValue: 1,
                duration: 300,
                useNativeDriver: true
            }).start();
        } else {
            Animated.timing(animation, {
                toValue: 0,
                duration: 300,
                useNativeDriver: true
            }).start();
        }
        setCollapsed(!collapsed);
    };

    const heightInterpolate = animation.interpolate({
        inputRange: [0, 1],
        outputRange: [0, 200]
    });

    return (
        <View style={styles.container}>
            <TouchableWithoutFeedback onPress={toggleCollapse}>
                <View style={styles.header}>
                    <Text style={styles.main}>{title}</Text>
                </View>
            </TouchableWithoutFeedback>
            <Animated.View style={{ height: heightInterpolate }}>
                <FlatList
                    data={songs}
                    renderItem={({ item }) => (
                        <View style={styles.itemContainer}>
                            <Text style={styles.title}>{item.title}</Text>
                            <Pressable onLongPress={() => onDelete(item.id)}>
                                <Image source={require('../assets/x-mark.png')} style={styles.deleteMark}/>
                            </Pressable>
                        </View>
                    )}
                    keyExtractor={item => item.id}
                    contentContainerStyle={styles.titleContainer}
                />
            </Animated.View>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        zIndex: 10,
    },
    header: {
        padding: 16,
        backgroundColor: COLORS.accent,
        alignItems: 'center',
        cursor: 'pointer',
    },
    main: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#fff',
    },
    itemContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingVertical: 8,
        paddingHorizontal: 16,
    },
    title: {
        fontSize: 18,
        fontWeight: 'bold',
        color: COLORS.black,
        marginBottom: 8,
        marginRight: 12,
    },
    titleContainer: {
        alignItems: 'center',
    },
    deleteMark: {
        width: 20,
        height: 20,
        marginBottom: 5,
    }
})